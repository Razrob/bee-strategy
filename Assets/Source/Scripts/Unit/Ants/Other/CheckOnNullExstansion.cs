public static class CheckOnNullExstansion
{
    /// <returns> return true if value is c# null or Unity null, else return false </returns>
    public static bool CheckOnNullAndUnityNull<T>(this T value) => value == null || ((value is UnityEngine.Object) && (value as UnityEngine.Object) == null);
}