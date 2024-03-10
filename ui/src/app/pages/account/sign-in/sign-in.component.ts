import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'sign-in',
  templateUrl: 'sign-in.component.html',
  styleUrls: ['sign-in.component.scss'],
})
export class SignInComponent implements OnInit {
  public containerClass: string[] = ['container'];
  ngOnInit(): void {}

  public toSignUp() {
    this.containerClass.push('sign-up-mode');
  }
  public toSignIn() {
    this.containerClass.filter((item) => item !== 'sign-up-mode');
  }
}
