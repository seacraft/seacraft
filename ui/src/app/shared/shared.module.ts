import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TranslateModule, TranslateStore } from '@ngx-translate/core';
import { IconsProviderModule } from '../icons-provider.module';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { SHARED_ZORRO_MODULES } from './shared-zorro.module';

@NgModule({
  imports: [
    TranslateModule.forChild({
        extend: true,
    }),
    FormsModule,
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    ...SHARED_ZORRO_MODULES,
  ],
  declarations: [
    PageNotFoundComponent,
  ],
  exports: [
    TranslateModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IconsProviderModule,
    PageNotFoundComponent,
    ...SHARED_ZORRO_MODULES,
  ],
  providers: [],
})
export class SharedModule {}

// this module is only for testing, you should only import this module in *.spec.ts files
@NgModule({
    imports: [
        BrowserAnimationsModule,
        SharedModule,
        HttpClientTestingModule,
        RouterTestingModule,
    ],
    exports: [
        BrowserAnimationsModule,
        SharedModule,
        HttpClientTestingModule,
        RouterTestingModule,
    ],
    providers: [
        TranslateStore
    ],
})
export class SharedTestingModule {}
