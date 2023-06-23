
import { NzTableQueryParams } from 'ng-zorro-antd/table';

import { Component, OnInit } from '@angular/core';

import { Option } from '../../../../shared/commonType';
import { RandomUser, RandomUserService } from './random-user-service';

@Component({
  selector: 'app-table3',
  templateUrl: './table3.component.html',
  styleUrls: ['./table3.component.less']
})
export default class Table3Component implements OnInit {
  loading = true;
  total = 1;
  listOfRandomUser: RandomUser[] = [];
  pageSize = 10;
  pageIndex = 1;
  filterGender = [
    { text: 'male', value: 'male' },
    { text: 'female', value: 'female' }
  ];

  loadDataFromServer(
    pageIndex: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    filter: Array<{ key: string; value: string[] }>
  ): void {
    this.loading = true;
    this.randomUserService.getUsers(pageIndex, pageSize, sortField, sortOrder, filter).subscribe(data => {
      this.loading = false;
      this.total = 200; // mock the total data here
      this.listOfRandomUser = data.results;
    });
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    console.log(params);
    const { pageSize, pageIndex, sort, filter } = params;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = (currentSort && currentSort.key) || null;
    const sortOrder = (currentSort && currentSort.value) || null;
    this.loadDataFromServer(pageIndex, pageSize, sortField, sortOrder, filter);
  }

  constructor(private randomUserService: RandomUserService) {}

  ngOnInit(): void {
    this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, []);
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



