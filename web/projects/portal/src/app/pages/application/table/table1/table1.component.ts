
import { Component, OnInit } from '@angular/core';

import { Option } from '../../../../shared/commonType';

@Component({
  selector: 'app-table1',
  templateUrl: './table1.component.html',
  styleUrls: ['./table1.component.less']
})
export class Table1Component implements OnInit {
  checked = false;
  loading = false;
  indeterminate = false;
  listOfData: readonly Data[] = [];
  listOfCurrentPageData: readonly Data[] = [];
  setOfCheckedId = new Set<number>();
  typeOptions: Option[]= [{name:'All',value:0},{name:'Product',value:1},{name:'Component',value:2},{name:'middleware',value:3}];
  queryObject:any={
    id:null,
    name:'',
    age:null,
    type:null

  };

  ngOnInit(): void {
    this.listOfData = new Array(100).fill(0).map((_, index) => ({
      id: index,
      name: `Edward King ${index}`,
      age: 32,
      type: this.getType(index),
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
  updateCheckedSet(id: number, checked: boolean): void {
    if (checked) {
      this.setOfCheckedId.add(id);
    }
 else {
      this.setOfCheckedId.delete(id);
    }
  }
  
  onCurrentPageDataChange(listOfCurrentPageData: readonly Data[]): void {
    this.listOfCurrentPageData = listOfCurrentPageData;
    this.refreshCheckedStatus();
  }
  
  refreshCheckedStatus(): void {
    const listOfEnabledData = this.listOfCurrentPageData.filter(({ disabled }) => !disabled);
    this.checked = listOfEnabledData.every(({ id }) => this.setOfCheckedId.has(id));
    this.indeterminate = listOfEnabledData.some(({ id }) => this.setOfCheckedId.has(id)) && !this.checked;
  }
  
  onItemChecked(id: number, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.refreshCheckedStatus();
  }
  
  onAllChecked(checked: boolean): void {
    this.listOfCurrentPageData
      .filter(({ disabled }) => !disabled)
      .forEach(({ id }) => this.updateCheckedSet(id, checked));
    this.refreshCheckedStatus();
  }
  
  search(){

  }
  
}
 export interface Data {
  id: number;
  name: string;
  age: number;
  type:Option;
  address: string;
  disabled: boolean;
}



