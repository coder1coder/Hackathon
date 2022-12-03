import {Component, OnInit} from "@angular/core";
import {AuthService} from "../../services/auth.service";
import {RouterService} from "../../services/router.service";
import {MatDialog} from "@angular/material/dialog";
import {CustomDialog, ICustomDialogData} from "../custom/custom-dialog/custom-dialog.component";

@Component({
  selector: 'toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})

export class ToolbarComponent implements OnInit {

  UserName:string | undefined;
  userId:number | undefined;

  constructor(
    private authService:AuthService,
    private routerService:RouterService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {

    this.authService.getCurrentUser()?.subscribe(x=>{
      if (x != null)
      {
        this.userId = x.id;
        this.UserName = x.fullName ?? x.userName
      }
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
