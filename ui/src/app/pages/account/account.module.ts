import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "src/app/shared/shared.module";
import { SignUpComponent } from "./sign-up/sign-up.component";
import { SignInComponent } from "./sign-in/sign-in.component";
import { SignInGuard } from "src/app/shared/router-guard/sign-in-guard-activate.service";

const routes: Routes = [
    {
        path: 'sign-in',
        // canActivate: [SignInGuard],
        component: SignInComponent,
    },
];
@NgModule({
    imports: [RouterModule.forChild(routes), SharedModule],
    declarations: [
        SignUpComponent,
        SignInComponent,
    ],
    providers: [],
})
export class AccountModule {}