import {Component, Input, OnInit} from '@angular/core';
import {SafeUrl} from "@angular/platform-browser";
import {FileStorageService} from "../../../services/file-storage.service";

@Component({
  selector: 'image-from-storage',
  templateUrl: './image-from-storage.component.html',
  styleUrls: ['./image-from-storage.component.scss']
})
export class ImageFromStorageComponent implements OnInit {

  @Input() imageId: string | undefined;
  imageUrl:SafeUrl;

  constructor(
    private fileStorageService: FileStorageService
  ) { }

  ngOnInit(): void {
    this.setSafeUrl();
  }

  private setSafeUrl(): void {
    if (this.imageId){
      this.fileStorageService.getById(this.imageId)
        .subscribe({next: (url:SafeUrl) =>{
            this.imageUrl = url
          }})
    }
  }
}
