namespace Hackathon.FileStorage.Abstraction.Models;

public record ProfileFileImage(int Width, int Height, long Length, string Extension)
    : FileImage(Width, Height, Length, Extension);
