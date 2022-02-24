import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {Actions} from "../common/Actions";
import {MatSnackBar} from "@angular/material/snack-bar";

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
