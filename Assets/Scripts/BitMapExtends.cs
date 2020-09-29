

using System;
using System.Drawing;
using System.Drawing.Imaging;


public struct BitmapMessage
{
    public BitmapMessage(Bitmap bitMap, IntPtr intPtr)
    {
        this.bitMap = bitMap;
        this.intPtr = intPtr;
    }

    public Bitmap bitMap;
    public IntPtr intPtr;
}
public static class BitMapExtends
{
    public static IntPtr BitMap2IntPtr(this Bitmap bmp)
    {
        bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
        Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
        BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
        IntPtr Iptr = bitmapData.Scan0;
        bmp.UnlockBits(bitmapData);     
        return Iptr;
    }     
}
