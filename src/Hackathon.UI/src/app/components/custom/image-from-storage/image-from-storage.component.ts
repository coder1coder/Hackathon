import { Component, Input, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { Subject, takeUntil } from 'rxjs';
import { FileStorageClient } from 'src/app/clients/file-storage.client';

@Component({
  selector: 'image-from-storage',
  templateUrl: './image-from-storage.component.html',
  styleUrls: ['./image-from-storage.component.scss'],
})
export class ImageFromStorageComponent implements OnInit {
  @Input() imageId: string;
  public imageUrl: SafeUrl;

  private destroy$ = new Subject();

  constructor(private fileStorageClient: FileStorageClient) {}

  public ngOnInit(): void {
    this.setSafeUrl();
  }

  private setSafeUrl(): void {
    if (this.imageId) {
      this.fileStorageClient
        .getById(this.imageId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({ next: (url: SafeUrl) => (this.imageUrl = url) });
    }
  }
}
