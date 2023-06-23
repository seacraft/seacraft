import { Component, OnInit } from '@angular/core';

import { SvgIconComponent } from "@seacraft/app-shared";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
  isCollapsed = false;
  listOfMenu: readonly Menu[] = [];
  currentSelectedOfBreadcrumb: readonly Breadcrumb[] =[];
  ngOnInit(): void {
    this.initListOfMenu();
  }

  private initListOfMenu(){
  
    const dashboard = {
      name: "Dashboard",
      icon:"dashboard",
      isOpen: false,
      router:"",
      breadcrumb:[],
      children:[]
    };
    const pages = {
      name: "Pages",
      icon:"form",
      isOpen: false,
      router:"",
      breadcrumb:[],
      children:[]
    };
    const applications = {
      name: "Applications",
      icon:"appstore",
      isOpen: true,
      router:"",
      breadcrumb:[],
      children:[
        {
          name: "Table",
          icon:"",
          isOpen: true,
          router:"/table-custom",
          children:[],
          breadcrumb:[
            {
              name:"Applications",
              order:0,
              router:"",
              icon: "appstore",
            },
            {
              name:"Table",
              order:1,
              router:"",
              icon: "",
            }
          ],
        },
        {
          name: "List",
          icon:"",
          isOpen: true,
          router:"/list-custom",
          children:[],
          breadcrumb:[
            {
              name:"Applications",
              order:0,
              router:"",
              icon: "appstore",
            },
            {
              name:"List",
              order:1,
              router:"",
              icon: "",
            }
          ],
        }
      ]
    };

    const arr = new Array<Menu>();
    arr.push(dashboard)
    arr.push(pages)
    arr.push(applications)
    
    const openMenu= arr.find(q=>q.isOpen);
      if(openMenu!=null){
          this.breadcrumbOfSelected(openMenu.children);
      }
     this.listOfMenu = arr;
  }

 public onRouter(menu:Menu){
    this.currentSelectedOfBreadcrumb = menu.breadcrumb.sort(q=>q.order);
  }
  
  private breadcrumbOfSelected(menuList:Menu[]){
    if(menuList == null){
      return;
    }
    const children = menuList.find(q=>q.isOpen);
    if(children!=null){
      this.currentSelectedOfBreadcrumb =children.breadcrumb.sort(q=>q.order);
    }
  }

 public onCollapsed(){
    this.isCollapsed = !this.isCollapsed;
  }
  
}

interface Menu{
  name: string
  icon: string
  isOpen: boolean
  router: string
  breadcrumb:Breadcrumb[]
  children:Menu[]
}
interface Breadcrumb{
   name:string
   order:number
   router: string
   icon: string
}