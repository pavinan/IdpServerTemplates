import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Auth/auth.service';

@Component({
  selector: 'app-sign-in-callback',
  templateUrl: './sign-in-callback.component.html',
  styleUrls: ['./sign-in-callback.component.scss']
})
export class SignInCallbackComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.signinCallback();
  }

}
