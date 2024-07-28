const fs = require('fs');
const xml2js = require('xml2js');
const { context, getOctokit } = require('@actions/github');

const github = getOctokit(process.env.GITHUB_TOKEN);

async function run() {
  try {
    const playmodeResults = await parseXML('artifacts/Coverage results for playmode/Report/Summary.xml');
    const editmodeResults = await parseXML('artifacts/Coverage results for editmode/Report/Summary.xml');

    const combinedResults = combineResults(playmodeResults, editmodeResults);
    const commentBody = createCommentBody(playmodeResults, editmodeResults, combinedResults);

    await deleteExistingCommentAndCreateNew(commentBody);
  } catch (error) {
    console.error('Error:', error);
    process.exit(1);
  }
}

function parseXML(filePath) {
  return new Promise((resolve, reject) => {
    const xml = fs.readFileSync(filePath, 'utf8');
    const parser = new xml2js.Parser();
    parser.parseString(xml, (err, result) => {
      if (err) reject(err);
      else resolve(result.CoverageReport.Summary[0]);
    });
  });
}

function combineResults(playmode, editmode) {
  return {
    Generatedon: [new Date().toISOString()],
    Author: ['soo-bak'],
    Assemblies: [parseInt(playmode.Assemblies[0])],
    Classes: [parseInt(playmode.Classes[0])],
    Files: [parseInt(playmode.Files[0])],
    Coveredlines: [parseInt(playmode.Coveredlines[0]) + parseInt(editmode.Coveredlines[0])],
    Uncoveredlines: [(parseInt(playmode.Uncoveredlines[0]) - parseInt(editmode.Coveredlines[0])) +
                     (parseInt(editmode.Uncoveredlines[0]) - parseInt(playmode.Coveredlines[0]))],
    Coverablelines: [parseInt(playmode.Coverablelines[0])],
    Totallines: [parseInt(playmode.Totallines[0])],
    Coveredmethods: [parseInt(playmode.Coveredmethods[0]) + parseInt(editmode.Coveredmethods[0])],
    Totalmethods: [parseInt(playmode.Totalmethods[0])],
  };
}

function createCommentBody(playmodeResults, editmodeResults, summary) {
  const playModeLineCoverage = (playmodeResults.Coveredlines[0] / playmodeResults.Coverablelines[0] * 100).toFixed(1);
  const EditModeLineCoverage = (editmodeResults.Coveredlines[0] / editmodeResults.Coverablelines[0] * 100).toFixed(1);

  const playModeMethodCoverage = (playmodeResults.Coveredmethods[0] / playmodeResults.Totalmethods[0] * 100).toFixed(1);
  const editModeMethodCoverage = (editmodeResults.Coveredmethods[0] / editmodeResults.Totalmethods[0] * 100).toFixed(1);

  const lineCoverage = (summary.Coveredlines[0] / summary.Coverablelines[0] * 100).toFixed(1);
  const methodCoverage = (summary.Coveredmethods[0] / summary.Totalmethods[0] * 100).toFixed(1);

  return `
  ### 🍉 Code Coverage Summary 🧐

  | Metric               | Value                        | Mode Details    |
  |----------------------|------------------------------|----------|
  | Generated on         | ${summary.Generatedon[0]}    | -        |
  | Author               | ${summary.Author[0]}         | -        |
  | Assemblies           | ${summary.Assemblies[0]}     | -        |
  | Classes              | ${summary.Classes[0]}        | -        |
  | Files                | ${summary.Files[0]}          | -        |
  | Covered Lines        | **${summary.Coveredlines[0]}**   | **${playmodeResults.Coveredlines[0]}** - Play Mode<br> **${editmodeResults.Coveredlines[0]}** - Edit Mode |
  | Uncovered Lines      | ${summary.Uncoveredlines[0]} | -        |
  | Coverable Lines      | **${summary.Coverablelines[0]}** | -        |
  | Total Lines          | ${summary.Totallines[0]}     | -        |
  | Line Coverage        | ${formatCoverage(lineCoverage)}     | ${formatCoverage(playModeLineCoverage)} - Play Mode<br> ${formatCoverage(EditModeLineCoverage)} - Edit Mode |
  | Covered Methods      | **${summary.Coveredmethods[0]}** | **${playmodeResults.Coveredmethods[0]}** - Play Mode<br> **${editmodeResults.Coveredmethods[0]}** - Edit Mode |
  | Total Methods        | **${summary.Totalmethods[0]}**   | -        |
  | Method Coverage      | ${formatCoverage(methodCoverage)}   | ${formatCoverage(playModeMethodCoverage)} - Play Mode<br> ${formatCoverage(editModeMethodCoverage)} - Edit Mode |
  `;
}

function formatCoverage(coverage) {
  const coverageNum = parseFloat(coverage);
  if (coverageNum === 100) {
    return `$\\{\\\\rm{\\color{#00FF00}100\\\\%}}$`;
  } else if (coverageNum >= 80) {
    return `$\\{\\\\rm{\\color{#FFD700}${coverageNum}\\\\%}}$`;
  } else {
    return `$\\{\\\\rm{\\color{#8B0000}${coverageNum}\\\\%}}$`;
  }
}

async function deleteExistingCommentAndCreateNew(body) {
  const { data: comments } = await github.rest.issues.listComments({
    ...context.repo,
    issue_number: context.issue.number,
  });

  const botComment = comments.find(comment =>
    comment.user.login === 'github-actions[bot]' &&
    comment.body.includes('Combined Code Coverage Summary')
  );

  if (botComment) {
    await github.rest.issues.deleteComment({
      ...context.repo,
      comment_id: botComment.id,
    });
    console.log('Deleted existing comment');
  }

  try {
    await github.rest.issues.createComment({
      ...context.repo,
      issue_number: context.issue.number,
      body,
    });
    console.log('Created new comment');
  } catch (error) {
    console.error('Error creating comment:', error);
    process.exit(1);
  }
}

run();
