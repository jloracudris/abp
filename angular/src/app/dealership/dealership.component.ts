import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-dealership',
  templateUrl: './dealership.component.html',
  styleUrls: ['./dealership.component.scss'],
})
export class DealerShipComponent {
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(private oAuthService: OAuthService, private authService: AuthService) {}

  login() {
    this.authService.navigateToLogin();
  }
}
