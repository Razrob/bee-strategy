public static class CheckOnNullExstension
{
    /// <summary>
    /// Method primarily for checking interfaces for null
    /// </summary>
    /// <returns>
    /// return true if value is c# null or Unity null, else return false
    /// </returns>
    public static bool IsNullOrUnityNull<T>(this T value) => value == null || ((value is UnityEngine.Object) && (value as UnityEngine.Object) == null);
}