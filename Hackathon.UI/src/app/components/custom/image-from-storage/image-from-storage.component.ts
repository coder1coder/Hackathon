import { Component, Input, OnInit } from '@angular/core';
import { SafeUrl } from "@angular/platform-browser";
import { FileStorageService } from "../../../services/file-storage.service";
import { Subject, takeUntil } from "rxjs";

@Component({
  selector: 'image-from-storage',
  templateUrl: './image-from-storage.component.html',
  styleUrls: ['./image-from-storage.component.scss'],
})
export class ImageFromStorageComponent implements OnInit {

  @Input() imageId: string;
  public imageUrl: SafeUrl;

  private destroy$ = new Subject();

  constructor(private fileStorageService: FileStorageService) {}

  public ngOnInit(): void {
    this.setSafeUrl();
  }

  private setSafeUrl(): void {
    if (this.imageId) {
      this.fileStorageService.getById(this.imageId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({next: (url:SafeUrl) => this.imageUrl = url});
    }
  }
}
