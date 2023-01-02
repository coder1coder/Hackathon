export class FileUtils
{
  public static MaxFileSize: number = 2048;
  public static Divider: number = 1024;

  public static IsImage(file: File): boolean {
    return file && file['type'].split('/')[0] === 'image';
  }
}
