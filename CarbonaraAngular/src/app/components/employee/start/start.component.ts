import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ChangePasswordFormComponent } from 'src/app/reusables/change-password-form/change-password-form.component';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.scss']
})
export class StartComponent implements OnInit {

  static messages = [
    "Wir wünschen dir einen schönen Arbeitstag!",
    "Arbeiten. Arbeiten.",
    "Du bist hier nicht um Pausen zu machen.",
    "Betriebsräte sind schlecht für das Arbeitsklima.",
    "Wunder erleben nur diejenigen, die an Wunder glauben.",
    "Wir können den Wind nicht ändern, aber die Segel anders setzen.",
    "Auch aus Steinen, die in den Weg gelegt werden, kann man Schönes bauen.",
    "Um klar zu sehen, genügt oft ein Wechsel der Blickrichtung.",
    "Wunder erleben nur diejenigen, die an Wunder glauben.",
    "Unter Druck entstehen Diamanten.",
    "Ähnlichkeiten zu real existierenden Personen sind rein zufällig und unterliegen der künstlerischen Freiheit.",
    "Es ist zwar doppelte Arbeit aber ich würde er nicht doppelte Arbeit ansehen.",
    "Defacto ist das eine gute Anwendung."
  ]
  messageIndex = 0;

  constructor(
    public authenticationService: AuthenticationService,
    private dialogService: MatDialog,
  ) {
    this.messageIndex = Math.floor(Math.random()*StartComponent.messages.length);
   }

  ngOnInit(): void {
  }

  getName() : string {
    let person = this.authenticationService.getPerson();
    return person?.firstname + " " + person?.lastname;
  }

  getMotd() : string {
    
    return StartComponent.messages[this.messageIndex];
  }

  changePassword() {
    this.dialogService.open(ChangePasswordFormComponent, {  });

  }

}
