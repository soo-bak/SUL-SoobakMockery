using System;

public static class StringExtensions {
  public static string Reverse(this string str) {
    if (string.IsNullOrEmpty(str)) return str;

    char[] charArr = str.ToCharArray();
    Array.Reverse(charArr);
    return new string(charArr);
  }
}
