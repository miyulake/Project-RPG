public enum SurfaceType
{
    DEFAULT,
    BRICK,
    WOOD,
    GRASS,
    WATER,
    LAVA,
    STONE,
    FLESH
}

public static class SurfaceDatabase
{
    public static SurfaceType GetSurface(int layer)
    {
        return layer switch
        {
            3   => SurfaceType.BRICK,
            4   => SurfaceType.WATER,
            6   => SurfaceType.WOOD,
            7   => SurfaceType.GRASS,
            8   => SurfaceType.LAVA,
            9   => SurfaceType.STONE,
            10  => SurfaceType.FLESH,
            _   => SurfaceType.DEFAULT,
        };
    }
}