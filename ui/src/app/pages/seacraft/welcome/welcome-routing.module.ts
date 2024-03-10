import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './welcome.component';
import { SharedModule } from 'src/app/shared/shared.module';

const routes: Routes = [
  { path: '', component: WelcomeComponent },
];

@NgModule({
  imports: [SharedModule,RouterModule.forChild(routes)],
})
export class WelcomeModule { }
