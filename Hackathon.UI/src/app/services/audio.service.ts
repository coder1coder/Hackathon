import {Injectable} from "@angular/core";

@Injectable({ providedIn: 'root' })
export class AudioService
{
  constructor() {
  }

  playOnReceiveMessage(){
    setTimeout(()=>{
      this.playSound('recieve_notify')
    }, 200)
  }

  playSound(sound:string){
    sound = `../assets/sounds/${sound}.mp3`;
    sound && ( new Audio(sound) ).play()
  }
}
