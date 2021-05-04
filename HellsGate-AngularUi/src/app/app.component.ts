import { Component, OnInit} from '@angular/core';
import { AccountService } from './_services';
import { User } from './_models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'HellsGate-AngularUi';
  user: User;

  constructor(private accountService: AccountService) {
     this.accountService.user.subscribe(x => this.user = x);
   }
  ngOnInit(): void {

  }

  logout() {
    this.accountService.logout();
  }
}
