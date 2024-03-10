import { TestBed, inject } from '@angular/core/testing';
import { SharedTestingModule } from '../shared/shared.module';
import { SeacraftTranslateLoaderService } from './seacraft-translate-loader.service';

describe('ConfigService', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [SharedTestingModule],
            providers: [SeacraftTranslateLoaderService],
        });
    });

    it('should be created', inject(
        [SeacraftTranslateLoaderService],
        (service: SeacraftTranslateLoaderService) => {
            expect(service).toBeTruthy();
        }
    ));
});
