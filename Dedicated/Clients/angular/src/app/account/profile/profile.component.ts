import { Component, OnInit } from '@angular/core';
import { ApplicationUserModel, ProfileService } from 'src/app/shared/services/profile.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  applicationUser: ApplicationUserModel;

  constructor(private profileService: ProfileService) { }

  ngOnInit(): void {

    this.profileService.get().subscribe(x => this.applicationUser = x);

  }

}
