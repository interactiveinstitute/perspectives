using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCoordinateSettings", menuName = "Coordinates/Settings", order = 1)]
public class CoordinateSettings : ScriptableObject {

    public GisConvert.GridType gridType;
    public double northingOffset;
    public double eastingOffset;
}
