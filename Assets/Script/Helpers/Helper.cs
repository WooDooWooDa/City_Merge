using System;
using UnityEngine;

public static class Helper
{
    public static Quaternion RandomRotation()
    {
        var randomRotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        randomRotation = RoundRotation(randomRotation);
        return RoundRotation(randomRotation);
    }
    public static Quaternion RoundRotation(Quaternion rot)
    {
        float angle = Quaternion.Angle(rot, Quaternion.identity);
        float roundedAngle = Mathf.Round(angle / 90f) * 90f;
        return Quaternion.AngleAxis(roundedAngle, Vector3.up);
    }

}