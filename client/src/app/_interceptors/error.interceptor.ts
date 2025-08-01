import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

//ovo je funkcija nije klasa
//
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService); // nitofikacioni servis
  
  return next(req).pipe //posto next vraca observable moramo da koristimo pipe da bismo radili nesto nakon
  (
    catchError(error => {
      if(error){
        switch (error.status) {
          case 400:
            if(error.error.errors)  // error.error pristupamo objektu koji salje bekend, a taj objekat ima polje errors(niz poruka o validaciji-ako su neispravni podaci)
            {
              const modalStateErrors = [];
              for(const key in error.error.errors)
              {
                if(error.error.errors[key])
                {
                  modalStateErrors.push(error.error.errors[key])
                }
              }
              throw modalStateErrors.flat();
            }
            else
            {
              toastr.error(error.error, error.status);
            }
            break;
        
          case 401:
            toastr.error('Unauthorised', error.status);
            break;

          case 404:
            router.navigateByUrl('/not-found');
            break;

          case 500:
            const navigationExtras: NavigationExtras = {state: {error: error.error}};
            router.navigateByUrl('/server-error', navigationExtras);
            break;

          default:
            toastr.error('Something unexpected went wrong');
            break;
        }
      }
      throw error;
    })
  )
};
