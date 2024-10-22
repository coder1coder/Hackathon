namespace Hackathon.FileStorage.Abstraction.Models;

public record EventFileImage(int Width, int Height, long Length, string Extension)
    : FileImage(Width, Height, Length, Extension);
