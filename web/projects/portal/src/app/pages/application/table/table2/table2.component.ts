import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';

import { Component, OnInit } from '@angular/core';

import { Option } from '../../../../shared/commonType';
import { Data } from '../table1/table1.component';
import { CreateProductComponent } from './components/create-product/create-product.component';
import { EditProductComponent } from './components/edit-product/edit-product.component';

@Component({
  selector: 'app-table2',
  templateUrl: './table2.component.html',
  styleUrls: ['./table2.component.less']
})
export class Table2Component implements OnInit {
  constructor(
    private modal:NzModalService,
    private msg: NzMessageService,
    private notification:NzNotificationService){

  }
  listOfData:  Data[] = [];
  listOfCurrentPageData: readonly Data[] = [];
  typeOptions: Option[]= [{name:'All',value:0},{name:'Product',value:1},{name:'Component',value:2},{name:'middleware',value:3}];

  ngOnInit(): void {
    this.listOfData = new Array(100).fill(0).map((_, index) => ({
      id: index,
      name: `Edward King ${index}`,
      age: 32,
      type:this.getType(index),
      address: `London, Park Lane no. ${index}`,
      disabled: index % 2 === 0
    }));
  }
  getType(index:number):Option{
    const n = index.toString();
    if(n.indexOf('3')){
      return this.typeOptions[1];
    }
else if (n.indexOf('8')){
      return this.typeOptions[2];
    }
    return   this.typeOptions[3];
  }
  onCurrentPageDataChange(listOfCurrentPageData: readonly Data[]): void {
    this.listOfCurrentPageData = listOfCurrentPageData;
  }

  onEdit(data:Data):void{
     this.modal.create({
        nzTitle:'Edit',
        nzContent:EditProductComponent,
        nzComponentParams:{
          data:data
        },
        nzOnOk:(ret)=>{
           this.notification.success('feedback!','edit success!',{nzDuration:0});
        },

      });
  }
  onAdd(){
    this.modal.create({
      nzTitle:'add',
      nzContent:CreateProductComponent,
      nzOnOk:(ret)=>{
          const item = ret?.data;
          item.id = this.listOfData.length+1;
          this.listOfData.unshift(item);
          this.listOfData=[...this.listOfData];

         this.notification.success('feedback!','add success!');
      },
    });
  }
  onDelete(id:number):void{
    this.listOfData=this.listOfData.filter(q=>q.id!==id);
    this.notification.success('feedback!','delete success!');
  }
  
}

