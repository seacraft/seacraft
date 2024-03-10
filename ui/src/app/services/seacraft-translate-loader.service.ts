import { Injectable } from '@angular/core';
import { TranslateLoader } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export class SeacraftTranslateLoaderService extends TranslateLoader {
    constructor(private http: HttpClient) {
        super();
    }
    getTranslation(lang: string): Observable<any> {
        const prefix: string = 'assets/i18n/lang/';
        let suffix: string = '-lang.json';
        return this.http.get(`${prefix}${lang}${suffix}`);
    }
}
