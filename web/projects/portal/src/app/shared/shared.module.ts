import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzSliderModule } from 'ng-zorro-antd/slider';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import {  NzFormModule } from 'ng-zorro-antd/form';

import { NzTableModule } from 'ng-zorro-antd/table';
import { NzListModule  } from 'ng-zorro-antd/list';
import { NzSkeletonModule  } from 'ng-zorro-antd/skeleton';


import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { IconsProviderModule } from '../icons-provider.module';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzNotificationServiceModule } from 'ng-zorro-antd/notification';

import { NzNotificationModule } from 'ng-zorro-antd/notification';


@NgModule({
    imports: [
      BrowserModule,
      BrowserAnimationsModule,
      FormsModule,
      ReactiveFormsModule,

      NzSpinModule,
      NzSliderModule,
      NzCardModule,
      NzSelectModule,
      NzInputModule,
      NzInputNumberModule,
      NzDividerModule,
      NzBreadCrumbModule,
      NzModalModule,
      NzNotificationModule,
      NzNotificationServiceModule,
      
      NzButtonModule,
      NzGridModule,
      NzAvatarModule,
      NzPopoverModule,
      NzMessageModule,
      NzPopconfirmModule,
      NzTableModule,
       NzFormModule,
       NzListModule,
       NzSkeletonModule,
      IconsProviderModule,
    ],
    declarations: [],
    exports: [
      
      ReactiveFormsModule,

      NzSpinModule,
      NzSliderModule,
      NzCardModule,
      NzSelectModule,
      NzInputModule,
      NzInputNumberModule,
      NzDividerModule,
      NzBreadCrumbModule,


      NzButtonModule,
      NzGridModule,
      NzAvatarModule,
      NzPopoverModule,
      NzMessageModule,
      NzPopconfirmModule,
      NzTableModule,
      NzListModule,
      NzSkeletonModule,
      
      NzModalModule,
      NzNotificationModule,
      NzNotificationServiceModule,

      NzFormModule,
      IconsProviderModule
    ],
    providers: [],
  })

export class SharedModule {}
