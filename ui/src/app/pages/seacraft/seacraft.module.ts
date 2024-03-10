import { RouterModule, Routes } from "@angular/router";
import { LayoutComponent } from "./layout/layout.component";
import { SharedModule } from "src/app/shared/shared.module";
import { NgModule } from "@angular/core";

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'welcome', pathMatch: 'full' },
            {
                path: 'welcome',
                loadChildren: () =>
                    import('./welcome/welcome-routing.module').then(
                        m => m.WelcomeModule
                    ),
            },
        ]
    }
]
@NgModule({
    imports: [SharedModule, RouterModule.forChild(routes)],
    declarations: [
        LayoutComponent,
    ],
})
export class SeacraftModule {}