import {AfterViewInit, Component, Input} from "@angular/core";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})

export class DefaultLayoutComponent implements AfterViewInit {

  @Input() title!: string;
  @Input() isLoading: boolean = false;

  UserName:string | undefined;

  constructor(private authService:AuthService) {
  }

  ngAfterViewInit(): void {
    this.authService.getCurrentUser()?.subscribe(x=>{
      if (x != null)
        this.UserName = x.fullName ?? x.userName
    })
  }
}

