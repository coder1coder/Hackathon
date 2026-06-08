import { Component, Input, OnInit, inject } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { Subject, takeUntil } from 'rxjs';
import { FileStorageClient } from 'src/app/clients/file-storage.client';

import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'image-from-storage',
    templateUrl: './image-from-storage.component.html',
    styleUrls: ['./image-from-storage.component.scss'],
    imports: [MatIcon]
})
export class ImageFromStorageComponent implements OnInit {
  private fileStorageClient = inject(FileStorageClient);

  @Input() imageId!: string | undefined;
  public imageUrl!: SafeUrl;

  private destroy$ = new Subject();



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
