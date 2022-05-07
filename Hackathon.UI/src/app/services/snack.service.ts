import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Actions} from "../common/Actions";

@Injectable({
  providedIn: 'root'
})

export class SnackService {

  api = environment.api;
  storage = sessionStorage;

  constructor(private snackBar:MatSnackBar) {
  }

  open(text:string, actions:Actions = Actions.OK, duration:number = 4000){
    this.snackBar.open(text, actions, {
      duration: duration
    });
  }
}
