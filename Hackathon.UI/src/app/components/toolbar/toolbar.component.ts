import {Component} from "@angular/core";
import {AuthService} from "../../services/auth.service";
import {RouterService} from "../../services/router.service";
import {MatDialog} from "@angular/material/dialog";
import {CustomDialog, ICustomDialogData} from "../custom/custom-dialog/custom-dialog.component";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})

export class ToolbarComponent {

  UserName:string | undefined;

  constructor(
    private authService:AuthService,
    private routerService:RouterService,
    private dialog: MatDialog
  ) {}

  ngAfterViewInit(): void {
    this.authService.getCurrentUser()?.subscribe(x=>{
      if (x != null)
        this.UserName = x.fullName ?? x.userName
    })
  }

  public logout(){
    let data: ICustomDialogData = {
      header: 'Выход',
      content: `Вы уверены, что хотите выйти?`,
      acceptButtonText: `Да`
    };

    this.dialog.open(CustomDialog, { data })
      .afterClosed()
      .subscribe(x =>
      {
        if (x) this.routerService.Profile.Logout();
      });
  }
}
