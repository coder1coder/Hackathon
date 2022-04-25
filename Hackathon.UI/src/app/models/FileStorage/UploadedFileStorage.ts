export class UploadedFileStorage
{
  id!: string;
  bucketName!: string;
  fileName! : string;
  filePath!: string;
  mimeType!: string;
  length!: number;
  ownerId?: number;
}
