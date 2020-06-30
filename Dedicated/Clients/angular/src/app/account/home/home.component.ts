import { Component, OnInit } from '@angular/core';
import { ProfileService, ApplicationUserModel } from 'src/app/shared/services/profile.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  applicationUser: ApplicationUserModel;

  constructor(private profileService: ProfileService) { }

  ngOnInit(): void {

    this.profileService.get().subscribe(x => this.applicationUser = x);

  }

}
