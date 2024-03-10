import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  
  { path: '', pathMatch: 'full', redirectTo: '/seacraft' },
  { path: 'account',
   loadChildren: () => import('./pages/account/account.module').then(m => m.AccountModule)
   },
  {
    path: 'seacraft',
    loadChildren: () =>
        import('./pages/seacraft/seacraft.module').then(m => m.SeacraftModule),
},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class SeacraftRoutingModule { }
