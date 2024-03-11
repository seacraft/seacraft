import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { DebugElement } from '@angular/core';

import {
    QuotaUnit,
    QuotaUnits,
    StorageMultipleConstant,
} from '../entities/shared.const';
import { AbstractControl } from '@angular/forms';
import { isValidCron } from 'cron-validator';
import { ClrDatagridStateInterface } from '@clr/angular';
import {
    HttpOptionInterface,
    HttpOptionTextInterface,
    QuotaUnitInterface, } from '../services/interface';
import { RequestQueryParams } from '../services/RequestQueryParams';

/**
 * Api levels
 */
enum APILevels {
    V1 = '',
    V2 = '/v2.0',
}

/**
 * v1 base href
 */
export const V1_BASE_HREF = '/api' + APILevels.V1;

/**
 * Current base href
 */
export const CURRENT_BASE_HREF = '/api' + APILevels.V2;

/**
 * Convert the different async channels to the Promise<T> type.
 *
 **
 * template T
 *  ** deprecated param {(Observable<T> | Promise<T> | T)} async
 * returns {Promise<T>}
 */
export function toPromise<T>(
    async: Observable<T> | Promise<T> | T
): Promise<T> {
    if (!async) {
        return Promise.reject('Bad argument');
    }

    if (async instanceof Observable) {
        let obs: Observable<T> = async;
        return obs.toPromise();
    } else {
        return Promise.resolve(async);
    }
}

export const HTTP_JSON_OPTIONS: HttpOptionInterface = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
    }),
    responseType: 'json',
};

export const HTTP_GET_OPTIONS: HttpOptionInterface = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        'Cache-Control': 'no-cache',
        Pragma: 'no-cache',
    }),
    responseType: 'json',
};
export const HTTP_GET_OPTIONS_OBSERVE_RESPONSE: HttpOptionInterface = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        'Cache-Control': 'no-cache',
        Pragma: 'no-cache',
    }),
    observe: 'response' as 'body',
    responseType: 'json',
};
export const HTTP_GET_OPTIONS_TEXT: HttpOptionTextInterface = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        'Cache-Control': 'no-cache',
        Pragma: 'no-cache',
    }),
    responseType: 'text',
};

export const HTTP_FORM_OPTIONS: HttpOptionInterface = {
    headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded',
    }),
    responseType: 'json',
};

export const HTTP_GET_HEADER: HttpHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    Accept: 'application/json',
    'Cache-Control': 'no-cache',
    Pragma: 'no-cache',
});

export const HTTP_GET_OPTIONS_CACHE: HttpOptionInterface = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        'Cache-Control': 'no-cache',
        Pragma: 'no-cache',
    }),
    responseType: 'json',
};

export const FILE_UPLOAD_OPTION: HttpOptionInterface = {
    headers: new HttpHeaders({
        'Content-Type': 'multipart/form-data',
    }),
    responseType: 'json',
};

/**
 * Build http request options
 *
 **
 *  ** deprecated param {RequestQueryParams} params
 * returns {RequestOptions}
 */
export function buildHttpRequestOptions(
    params: RequestQueryParams
): HttpOptionInterface {
    let reqOptions: HttpOptionInterface = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Accept: 'application/json',
            'Cache-Control': 'no-cache',
            Pragma: 'no-cache',
        }),
        responseType: 'json',
    };
    if (params) {
        reqOptions.params = params;
    }

    return reqOptions;
}
export function buildHttpRequestOptionsWithObserveResponse(
    params: RequestQueryParams
): HttpOptionInterface {
    let reqOptions: HttpOptionInterface = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Accept: 'application/json',
            'Cache-Control': 'no-cache',
            Pragma: 'no-cache',
        }),
        responseType: 'json',
        observe: 'response' as 'body',
    };
    if (params) {
        reqOptions.params = params;
    }
    return reqOptions;
}

/** Button events to pass to `DebugElement.triggerEventHandler` for RouterLink event handler */
export const ButtonClickEvents = {
    left: { button: 0 },
    right: { button: 2 },
};

/** Simulate element click. Defaults to mouse left-button click event. */
export function click(
    el: DebugElement | HTMLElement,
    eventObj: any = ButtonClickEvents.left
): void {
    if (el instanceof HTMLElement) {
        el.click();
    } else {
        el.triggerEventHandler('click', eventObj);
    }
}


/**
 * Filter columns via RegExp
 *
 **
 *  ** deprecated param {State} state
 * returns {void}
 */
export function doFiltering<T extends { [key: string]: any | any[] }>(
    items: T[],
    state: State
): T[] {
    if (!items || items.length === 0) {
        return items;
    }

    if (!state || !state.filters || state.filters.length === 0) {
        return items;
    }

    state.filters.forEach((filter: { property: string; value: string }) => {
        items = items.filter(item => {
            if (filter['property'].indexOf('.') !== -1) {
                let arr = filter['property'].split('.');
                if (Array.isArray(item[arr[0]]) && item[arr[0]].length) {
                    return item[arr[0]].some((data: any) => {
                        return filter['value'] === data[arr[1]];
                    });
                }
            } else {
                return regexpFilter(filter['value'], item[filter['property']]);
            }
        });
    });

    return items;
}

/**
 * Match items via RegExp
 *
 **
 *  ** deprecated param {string} terms
 *  ** deprecated param {*} testedValue
 * returns {boolean}
 */
export function regexpFilter(terms: string, testedValue: any): boolean {
    let reg = new RegExp('.*' + terms + '.*', 'i');
    return reg.test(testedValue);
}

/**
 * Sorting the data by column
 *
 **
 * template T
 *  ** deprecated param {T[]} items
 *  ** deprecated param {State} state
 * returns {T[]}
 */
export function doSorting<T extends { [key: string]: any | any[] }>(
    items: T[],
    state: State
): T[] {
    if (!items || items.length === 0) {
        return items;
    }
    if (!state || !state.sort) {
        return items;
    }

    return items.sort((a: T, b: T) => {
        let comp: number = 0;
        if (typeof state.sort.by !== 'string') {
            comp = state.sort.by.compare(a, b);
        } else {
            let propA = a[state.sort.by.toString()],
                propB = b[state.sort.by.toString()];
            if (typeof propA === 'string') {
                comp = propA.localeCompare(propB);
            } else {
                if (propA > propB) {
                    comp = 1;
                } else if (propA < propB) {
                    comp = -1;
                }
            }
        }

        if (state.sort.reverse) {
            comp = -comp;
        }

        return comp;
    });
}

/**
 * Compare the two objects to adjust if they're equal
 *
 **
 *  ** deprecated param {*} a
 *  ** deprecated param {*} b
 * returns {boolean}
 */
export function compareValue(a: any, b: any): boolean {
    if ((a && !b) || (!a && b)) {
        return false;
    }
    if (!a && !b) {
        return true;
    }

    return JSON.stringify(a) === JSON.stringify(b);
}

/**
 * Check if the object is null or empty '{}'
 *
 **
 *  ** deprecated param {*} obj
 * returns {boolean}
 */
export function isEmptyObject(obj: any): boolean {
    return !obj || JSON.stringify(obj) === '{}';
}

/**
 * Deeper clone all
 *
 **
 *  ** deprecated param {*} srcObj
 * returns {*}
 */
export function clone(srcObj: any): any {
    if (!srcObj) {
        return null;
    }
    return JSON.parse(JSON.stringify(srcObj));
}

export function isEmpty(obj: any): boolean {
    return !obj || JSON.stringify(obj) === '{}';
}

export function downloadFile(fileData) {
    let url = window.URL.createObjectURL(fileData.data);
    let a = document.createElement('a');
    document.body.appendChild(a);
    a.setAttribute('style', 'display: none');
    a.href = url;
    a.download = fileData.filename;
    a.click();
    window.URL.revokeObjectURL(url);
    a.remove();
}

export function getChanges(
    original: any,
    afterChange: any
): { [key: string]: any | any[] } {
    let changes: { [key: string]: any | any[] } = {};
    if (!afterChange || !original) {
        return changes;
    }
    for (let prop of Object.keys(afterChange)) {
        let field = original[prop];
        if (field && field.editable) {
            if (!compareValue(field.value, afterChange[prop].value)) {
                changes[prop] = afterChange[prop].value;
                // Number
                if (typeof field.value === 'number') {
                    changes[prop] = +changes[prop];
                }

                // Trim string value
                if (typeof field.value === 'string') {
                    changes[prop] = ('' + changes[prop]).trim();
                }
            }
        }
    }
    return changes;
}

/**
 * validate cron expressions
 * @param testValue
 */
export function cronRegex(testValue: any): boolean {
    // must have 6 fields
    if (testValue && testValue.trim().split(/\s+/g).length < 6) {
        return false;
    }
    return isValidCron(testValue, {
        seconds: true,
        alias: true,
        allowBlankDay: true,
    });
}

/**
 * Keep decimal digits
 * @param count number
 * @param decimals number 1、2、3 ···
 */
export const roundDecimals = (count, decimals = 0) => {
    return Number(`${Math.round(+`${count}e${decimals}`)}e-${decimals}`);
};
/**
 * get suitable unit
 * @param count number  ;bit
 * @param quotaUnitsDeep Array link  QuotaUnits;
 */
export const getSuitableUnit = (
    count: number,
    quotaUnitsDeep: QuotaUnitInterface[]
): string => {
    for (let unitObj of quotaUnitsDeep) {
        if (count / StorageMultipleConstant >= 1 && quotaUnitsDeep.length > 1) {
            quotaUnitsDeep.shift();
            return getSuitableUnit(
                count / StorageMultipleConstant,
                quotaUnitsDeep
            );
        } else {
            return +count
                ? `${roundDecimals(count, 2)}${unitObj.UNIT}`
                : `0${unitObj.UNIT}`;
        }
    }
    return `${roundDecimals(count, 2)}${QuotaUnits[0].UNIT}`;
};
/**
 * get byte from GB、MB、TB
 * @param count number
 * @param unit MB /GB / TB
 */
export const getByte = (count: number, unit: string): number => {
    let flagIndex;
    return QuotaUnits.reduce((totalValue, currentValue, index) => {
        if (currentValue.UNIT === unit) {
            flagIndex = index;
            return totalValue;
        } else {
            if (!flagIndex) {
                return totalValue * StorageMultipleConstant;
            }
            return totalValue;
        }
    }, count);
};
/**
 * get integet and unit  in hard storage and used storage;and the unit of used storage <= the unit of hard storage
 * @param hardNumber hard storage number
 * @param quotaUnitsDeep clone(Quotas)
 * @param usedNumber used storage number
 * @param quotaUnitsDeepClone clone(Quotas)
 */
export const GetIntegerAndUnit = (
    hardNumber: number,
    quotaUnitsDeep: QuotaUnitInterface[],
    usedNumber: number,
    quotaUnitsDeepClone: QuotaUnitInterface[]
) => {
    for (let unitObj of quotaUnitsDeep) {
        if (
            hardNumber % StorageMultipleConstant === 0 &&
            quotaUnitsDeep.length > 1
        ) {
            quotaUnitsDeep.shift();
            if (usedNumber / StorageMultipleConstant >= 1) {
                quotaUnitsDeepClone.shift();
                return GetIntegerAndUnit(
                    hardNumber / StorageMultipleConstant,
                    quotaUnitsDeep,
                    usedNumber / StorageMultipleConstant,
                    quotaUnitsDeepClone
                );
            } else {
                return GetIntegerAndUnit(
                    hardNumber / StorageMultipleConstant,
                    quotaUnitsDeep,
                    usedNumber,
                    quotaUnitsDeepClone
                );
            }
        } else {
            return {
                partNumberHard: +hardNumber,
                partCharacterHard: unitObj.UNIT,
                partNumberUsed: roundDecimals(+usedNumber, 2),
                partCharacterUsed: quotaUnitsDeepClone[0].UNIT,
            };
        }
    }
};

export const validateLimit = unitContrl => {
    return (control: AbstractControl) => {
        if (
            // 1024TB
            getByte(control.value, unitContrl.value) >
            Math.pow(StorageMultipleConstant, 5)
        ) {
            return {
                error: true,
            };
        }
        return null;
    };
};

export function formatSize(tagSize: string): string {
    const size: number = Number.parseInt(tagSize, 10);
    if (Math.pow(1024, 1) <= size && size < Math.pow(1024, 2)) {
        return (size / Math.pow(1024, 1)).toFixed(2) + 'KiB';
    } else if (Math.pow(1024, 2) <= size && size < Math.pow(1024, 3)) {
        return (size / Math.pow(1024, 2)).toFixed(2) + 'MiB';
    } else if (Math.pow(1024, 3) <= size && size < Math.pow(1024, 4)) {
        return (size / Math.pow(1024, 3)).toFixed(2) + 'GiB';
    } else if (Math.pow(1024, 4) <= size) {
        return (size / Math.pow(1024, 4)).toFixed(2) + 'TiB';
    } else {
        return size + 'B';
    }
}

/**
 * get size number of target size (in byte)
 * @param size
 */
export function getSizeNumber(size: number): string | number {
    if (Math.pow(1024, 1) <= size && size < Math.pow(1024, 2)) {
        return (size / Math.pow(1024, 1)).toFixed(2);
    } else if (Math.pow(1024, 2) <= size && size < Math.pow(1024, 3)) {
        return (size / Math.pow(1024, 2)).toFixed(2);
    } else if (Math.pow(1024, 3) <= size && size < Math.pow(1024, 4)) {
        return (size / Math.pow(1024, 3)).toFixed(2);
    } else if (Math.pow(1024, 4) <= size) {
        return (size / Math.pow(1024, 4)).toFixed(2);
    } else {
        return size;
    }
}

/**
 * get size unit of target size (in byte)
 * @param size
 */
export function getSizeUnit(size: number): string {
    if (Math.pow(1024, 1) <= size && size < Math.pow(1024, 2)) {
        return QuotaUnit.KB;
    } else if (Math.pow(1024, 2) <= size && size < Math.pow(1024, 3)) {
        return QuotaUnit.MB;
    } else if (Math.pow(1024, 3) <= size && size < Math.pow(1024, 4)) {
        return QuotaUnit.GB;
    } else if (Math.pow(1024, 4) <= size) {
        return QuotaUnit.TB;
    } else {
        return QuotaUnit.BIT;
    }
}

/**
 * Simple object check.
 * @param item
 * @returns {boolean}
 */
export function isObject(item): boolean {
    return item && typeof item === 'object' && !Array.isArray(item);
}

/**
 * Deep merge two objects.
 * @param target
 * @param ...sources
 */
export function mergeDeep(target, ...sources) {
    if (!sources.length) {
        return target;
    }
    const source = sources.shift();

    if (isObject(target) && isObject(source)) {
        for (const key in source) {
            if (isObject(source[key])) {
                if (!target[key]) {
                    Object.assign(target, { [key]: {} });
                }
                mergeDeep(target[key], source[key]);
            } else {
                Object.assign(target, { [key]: source[key] });
            }
        }
    }
    return mergeDeep(target, ...sources);
}
export function dbEncodeURIComponent(url: string) {
    if (typeof url === 'string') {
        return encodeURIComponent(encodeURIComponent(url));
    }
    return '';
}

/**
 * delete empty key
 * @param obj
 */
export function deleteEmptyKey(obj: Object): void {
    if (isEmptyObject(obj)) {
        return;
    }
    for (let key in obj) {
        if (!obj[key]) {
            delete obj[key];
        }
    }
}

/**
 * Get sorting string from  current state
 * @param state
 */
export function getSortingString(state: ClrDatagridStateInterface): string {
    if (state && state.sort && state.sort.by) {
        let sortString: string;
        if (typeof state.sort.by === 'string') {
            sortString = state.sort.by;
        } else {
            sortString = (state.sort.by as any).fieldName;
        }
        if (state.sort.reverse) {
            sortString = `-${sortString}`;
        }
        return sortString;
    }
    return null;
}

/**
 * Get query string from current state, rules as below:
 * query string format: q=k=v,k=~v,k=[min~max],k={v1 v2 v3},k=(v1 v2 v3)
 * exact match: k=v
 * fuzzy match: k=~v
 * range: k=[min~max]
 * or list: k={v1 v2 v3}
 * and list: k=(v1 v2 v3)
 * @param state
 */
export function getQueryString(state: ClrDatagridStateInterface): string {
    let str: string = '';
    if (state && state.filters && state.filters.length) {
        state.filters.forEach(item => {
            if (str) {
                str += `,${item.property}=~${item.value}`;
            } else {
                str += `${item.property}=~${item.value}`;
            }
        });
        return encodeURIComponent(str);
    }
    return null;
}

/**
 * if two object are the same
 * @param a
 * @param b
 */
export function isSameObject(a: any, b: any): boolean {
    if (a && !b) {
        return false;
    }
    if (b && !a) {
        return false;
    }
    if (a && b) {
        if (Array.isArray(a) || Array.isArray(b)) {
            return false;
        }
        const c: any = Object.keys(a).length > Object.keys(b).length ? a : b;
        for (const key in c) {
            if (c.hasOwnProperty(key)) {
                if (!c[key]) {
                    // should not use triple-equals here
                    // eslint-disable-next-line eqeqeq
                    if (a[key] != b[key]) {
                        return false;
                    }
                } else {
                    if (Array.isArray(c[key])) {
                        if (!isSameArrayValue(a[key], b[key])) {
                            return false;
                        }
                    } else if (isObject(c[key])) {
                        if (!isSameObject(a[key], b[key])) {
                            return false;
                        }
                    } else {
                        // should not use triple-equals here
                        // eslint-disable-next-line eqeqeq
                        if (a[key] != b[key]) {
                            return false;
                        }
                    }
                }
            }
        }
    }
    return true;
}

/**
 * if two arrays have the same length and contain the same items, they are regarded as the same
 * @param a
 * @param b
 */
export function isSameArrayValue(a: any, b: any): boolean {
    if (a && b && Array.isArray(a) && Array.isArray(a)) {
        if (a.length !== b.length) {
            return false;
        }
        let isSame: boolean = true;
        a.forEach(itemOfA => {
            let hasItem: boolean = false;
            b.forEach(itemOfB => {
                if (isSameObject(itemOfA, itemOfB)) {
                    hasItem = true;
                }
            });
            if (!hasItem) {
                isSame = false;
            }
        });
        if (isSame) {
            return true;
        }
    }
    return false;
}

/**
 * delete specified param from target url
 * @param url
 * @param key
 */
export function delUrlParam(url: string, key: string): string {
    if (url && url.indexOf('?') !== -1) {
        const baseUrl: string = url.split('?')[0];
        const query: string = url.split('?')[1];
        if (query.indexOf(key) > -1) {
            let obj = {};
            let arr: any[] = query.split('&');
            for (let i = 0; i < arr.length; i++) {
                arr[i] = arr[i].split('=');
                obj[arr[i][0]] = arr[i][1];
            }
            delete obj[key];
            if (!Object.keys(obj) || !Object.keys(obj).length) {
                return baseUrl;
            }
            return (
                baseUrl +
                '?' +
                JSON.stringify(obj)
                    .replace(/[\"\{\}]/g, '')
                    .replace(/\:/g, '=')
                    .replace(/\,/g, '&')
            );
        }
    }
    return url;
}

/**
 * Convert seconds to xx hrs xx min xx sec
 * @param distance in milliseconds
 */
export function durationStr(distance: number): string {
    const hours = Math.floor(distance / 3600000);
    distance -= hours * 3600000;
    const minutes = Math.floor(distance / 60000);
    distance -= minutes * 60000;
    const seconds = Math.floor(distance / 1000);
    let result: string = '';
    if (seconds) {
        result = `${seconds}sec`;
    }
    if (minutes) {
        result = `${minutes}min ${seconds}sec`;
    }
    if (hours) {
        result = `${hours}hrs ${minutes}min ${seconds}sec`;
    }
    return result ? result : '0';
}
