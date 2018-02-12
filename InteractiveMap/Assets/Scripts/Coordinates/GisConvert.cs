using System;

public class GisConvert
{
    public enum GridType
    {
        RT9025GonV, Sweref99L1630, Sweref99L1800, Undefined
    }

    //struct GridTypeName
    //{
    //    public string name;
    //    public GridType type;

    //    public GridTypeName(string _name, GridType _type)
    //    {
    //        name = _name;
    //        type = _type;
    //    }
    //}

    //public static GridTypeName[] gridTypeTable = {
    //                                   new GridTypeName("RT9025GonV", GridType.RT9025GonV),
    //                                   new GridTypeName("Sweref99L1630", GridType.Sweref99L1630),
    //                                   new GridTypeName("Sweref99L1800", GridType.Sweref99L1800)
    //                               };

    //public static GridType GridNameToType(string name)
    //{
    //    foreach (GridTypeName tableRow in gridTypeTable)
    //    {
    //        if (tableRow.name == name)
    //        {
    //            return tableRow.type;
    //        }
    //    }

    //    return GridType.Undefined;
    //}

    public struct ConversionParameters
    {
        public double axis;
        public double flattening;
        public double central_meridian;
        public double lat_of_origin;
        public double scale;
        public double false_northing;
        public double false_easting;
    };

    //static ConversionParameters MakeRT90Basic()
    //{
    //    ConversionParameters p;
    //    p.axis = 6378137.0; // GRS 80.
    //    p.flattening = 1.0 / 298.257222101; // GRS 80.
    //    p.central_meridian = 0.0;
    //    p.lat_of_origin = 0.0;

    //    return p;
    //}

    // National
    public static ConversionParameters MakeRT9025GonV()
    {
        ConversionParameters p;
        p.axis = 6378137.0; // GRS 80.
        p.flattening = 1.0 / 298.257222101; // GRS 80.
        p.central_meridian = 0.0;
        p.lat_of_origin = 0.0;
        p.central_meridian = 15.0 + 48.0 / 60.0 + 22.624306 / 3600.0;
        p.scale = 1.00000561024;
        p.false_northing = -667.711;
        p.false_easting = 1500064.274;
        return p;
    }

    static ConversionParameters MakeSweref99Basic()
    {
        ConversionParameters p;
        p.axis = 6378137.0; // GRS 80.
        p.flattening = 1.0 / 298.257222101; // GRS 80.
        p.central_meridian = 0.0;
        p.lat_of_origin = 0.0;
        p.scale = 1.0;
        p.false_northing = 0.0;
        p.false_easting = 150000.0;

        return p;
    }

    public static ConversionParameters MakeSweref99L1630()
    {
        ConversionParameters p = MakeSweref99Basic();
        p.central_meridian = 16.50;
        return p;
    }

    public static ConversionParameters MakeSweref99L1800()
    {
        ConversionParameters p = MakeSweref99Basic();
        p.central_meridian = 18.0;
        return p;
    }

    public static ConversionParameters MakeConversionParameters(GridType type)
    {
        switch (type)
        {
            case GridType.RT9025GonV:
                return MakeRT9025GonV();
            case GridType.Sweref99L1630:
                return MakeSweref99L1630();
            case GridType.Sweref99L1800:
                return MakeSweref99L1800();
            default:
                break;
        }
        return MakeSweref99Basic();
    }

    public static void GeodeticToGrid( double latitude, double longitude, ConversionParameters p, out double outNorthing, out double outEasting)
    {
        outNorthing = 0.0;
        outEasting = 0.0;

        double axis = p.axis;
        double flattening = p.flattening;
        double central_meridian = p.central_meridian;
        double lat_of_origin = p.lat_of_origin;
        double scale = p.scale;
        double false_northing = p.false_northing;
        double false_easting = p.false_easting;

        //double x, y;

        if (central_meridian == 0.0) {
            outNorthing = 0.0;
            outEasting = 0.0;
            return;
        }

        // Prepare ellipsoid-based stuff.
        double e2 = flattening * (2.0 - flattening);
        double n = flattening / (2.0 - flattening);
        double a_roof = axis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
        double A = e2;
        double B = (5.0 * e2 * e2 - e2 * e2 * e2) / 6.0;
        double C = (104.0 * e2 * e2 * e2 - 45.0 * e2 * e2 * e2 * e2) / 120.0;
        double D = (1237.0 * e2 * e2 * e2 * e2) / 1260.0;
        double beta1 = n / 2.0 - 2.0 * n * n / 3.0 + 5.0 * n * n * n / 16.0 + 41.0 * n * n * n * n / 180.0;
        double beta2 = 13.0 * n * n / 48.0 - 3.0 * n * n * n / 5.0 + 557.0 * n * n * n * n / 1440.0;
        double beta3 = 61.0 * n * n * n / 240.0 - 103.0 * n * n * n * n / 140.0;
        double beta4 = 49561.0 * n * n * n * n / 161280.0;

        // Convert.
        double deg_to_rad = Math.PI / 180.0;
        double phi = latitude * deg_to_rad;
        double lambda = longitude * deg_to_rad;
        double lambda_zero = central_meridian * deg_to_rad;

        double phi_star = phi - Math.Sin(phi) * Math.Cos(phi) * (A +
                        B * Math.Pow(Math.Sin(phi), 2) +
                        C * Math.Pow(Math.Sin(phi), 4) +
                        D * Math.Pow(Math.Sin(phi), 6));
        double delta_lambda = lambda - lambda_zero;
        double xi_prim = Math.Atan(Math.Tan(phi_star) / Math.Cos(delta_lambda));
        double eta_prim = Math.Atan(Math.Cos(phi_star) * Math.Sin(delta_lambda));
        double x = scale * a_roof * (xi_prim +
                        beta1 * Math.Sin(2.0 * xi_prim) * Math.Cos(2.0 * eta_prim) +
                        beta2 * Math.Sin(4.0 * xi_prim) * Math.Cos(4.0 * eta_prim) +
                        beta3 * Math.Sin(6.0 * xi_prim) * Math.Cos(6.0 * eta_prim) +
                        beta4 * Math.Sin(8.0 * xi_prim) * Math.Cos(8.0 * eta_prim)) +
                        false_northing;
        double y = scale * a_roof * (eta_prim +
                        beta1 * Math.Cos(2.0 * xi_prim) * Math.Sinh(2.0 * eta_prim) +
                        beta2 * Math.Cos(4.0 * xi_prim) * Math.Sinh(4.0 * eta_prim) +
                        beta3 * Math.Cos(6.0 * xi_prim) * Math.Sinh(6.0 * eta_prim) +
                        beta4 * Math.Cos(8.0 * xi_prim) * Math.Sinh(8.0 * eta_prim)) +
                        false_easting;
        outNorthing = x * 1000.0 / 1000.0;
        outEasting = y * 1000.0 / 1000.0;
    }
}
