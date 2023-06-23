import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CpListComponent } from './pages/application/list/cp-list.component';
import { CpTableComponent } from './pages/application/table/cp-table.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/list-custom' },
  { path: 'table-custom', component:CpTableComponent },
  { path: 'list-custom', component:CpListComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
