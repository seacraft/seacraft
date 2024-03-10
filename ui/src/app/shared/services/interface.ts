import { HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";


export interface ClrDatagridComparatorInterface<T> {
    compare(a: T, b: T): number;
}



export interface ClrDatagridFilterInterface<T> {
    isActive(): boolean;

    accepts(item: T): boolean;

    changes: Observable<any>;
}



export interface HttpOptionInterface {
    headers?:
        | HttpHeaders
        | {
              [header: string]: string | string[];
          };
    observe?: 'body';
    params?:
        | HttpParams
        | {
              [param: string]: string | string[];
          };
    reportProgress?: boolean;
    responseType: 'json';
    withCredentials?: boolean;
}

export interface HttpOptionTextInterface {
    headers?:
        | HttpHeaders
        | {
              [header: string]: string | string[];
          };
    observe?: 'body';
    params?:
        | HttpParams
        | {
              [param: string]: string | string[];
          };
    reportProgress?: boolean;
    responseType: 'text';
    withCredentials?: boolean;
}
export interface QuotaUnitInterface {
    UNIT: string;
}