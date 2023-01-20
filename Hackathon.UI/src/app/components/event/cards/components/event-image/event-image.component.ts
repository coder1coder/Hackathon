import {Component, Input, OnInit} from '@angular/core';
import {IEventListItem} from "../../../../../models/Event/IEventListItem";
import {SafeUrl} from "@angular/platform-browser";
import {Event} from 'src/app/models/Event/Event';
import {FileStorageService} from "../../../../../services/file-storage.service";

@Component({
  selector: 'app-event-image',
  templateUrl: './event-image.component.html',
  styleUrls: ['./event-image.component.scss']
})
export class EventImageComponent implements OnInit {

  @Input() event: IEventListItem | Event;

  constructor(
    private fileStorageService: FileStorageService
  ) { }

  ngOnInit(): void {
    this.setSafeUrl();
  }

  private setSafeUrl(): void {
    if (this.event.imageId){
      this.fileStorageService.getById(this.event.imageId)
        .subscribe({next: (url:SafeUrl) =>{
            this.event.imageUrl = url
          }})
    }
  }
}
