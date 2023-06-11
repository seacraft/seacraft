import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { SvgIconComponent } from './svg-icon/svg-icon.component';
import { BackdropBlurDirective } from './backdrop-blur/backdrop-blur.directive';

@NgModule({
    declarations: [
        SvgIconComponent,
        BackdropBlurDirective
    ],
    imports: [
        HttpClientModule
    ],
    exports: [
        SvgIconComponent,
        BackdropBlurDirective
    ]
})
export class AppSharedModule { }
