import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Certification, Education, Employment, Freelancer, ProfileSummary } from '../models/freelancer.model';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { AddCertificationCommand } from '../models/commands/add-certification-command.model';
import { Skill } from '../../shared/models/profession.mode';
import { AddEducationCommand } from '../models/commands/add-education-command.model';
import { AddEmploymentCommand } from '../models/commands/add-employment-command.model';
import { AddSkillCommand } from '../models/commands/add-skill-command.model';
import { ProfileSetupCommand } from '../models/commands/profile-setup-command.model';
import { EditCertificationCommand } from '../models/commands/edit-certification-command.model';
import { EditEducationCommand } from '../models/commands/edit-education-command.model';
import { EditEmploymentCommand } from '../models/commands/edit-employment-command.model';

@Injectable({
  providedIn: 'root'
})
export class FreelancerService {

  private profileSetupCompletedSource = new BehaviorSubject<boolean | undefined>(undefined);
  public profileSetupCompletedObserver = this.profileSetupCompletedSource.asObservable();

  currentFreelancer: Freelancer = new Freelancer();

  constructor(private httpClient: HttpClient) { }

  setupProfile(profileSetupCommand: ProfileSetupCommand): Observable<any> {
    return this.httpClient.put<Freelancer>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}`, profileSetupCommand)
      .pipe(
        map(() => this.profileSetupCompletedSource.next(true))
      );
  }

  get(freelancerId: string): Observable<Freelancer> {
    return this.httpClient.get<Freelancer>(`api/freelancer-service/freelancer/${freelancerId}`)
      .pipe(
        map((freelancer) => {
          this.currentFreelancer = freelancer;
          this.profileSetupCompletedSource.next(freelancer.profession !== null);
          return freelancer;
        })
      );
  }

  setProfilePicture(profilePicture: File): Observable<string> {
    const reader = new FileReader();

    return new Observable<string>((observer) => {
      reader.onloadend = () => {
        const pictureBase64 = reader.result?.toString().split(',')[1]; // Remove the `data:image/...;base64,` prefix
        const body = {
          pictureBase64: pictureBase64
        };
        this.httpClient
          .put<string>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/profile-picture`, body)
          .subscribe({
            next: (result) => {
              observer.next(result);
              observer.complete();
            },
            error: (error) => {
              observer.error(error);
            }
          });
      };

      reader.onerror = (error) => {
        observer.error(error);
      };

      reader.readAsDataURL(profilePicture);
    });
  }

  editProfileSummary(profileSummary: ProfileSummary): Observable<void> {
    return this.httpClient.put<ProfileSummary>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/profile-summary`, profileSummary)
      .pipe(
        map((profileSummary) => {
          this.currentFreelancer.profileSummary = profileSummary;
        })
      );
  }

  addEducation(addEducationCommand: AddEducationCommand): Observable<void> {
    return this.httpClient.post<Education>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/education`, addEducationCommand)
      .pipe(
        map((education) => {
          this.currentFreelancer.educations.push(education);
        })
      );
  }

  editEducation(editEducationCommand: EditEducationCommand): Observable<void> {
    const url = `api/freelancer-service/freelancer/${this.currentFreelancer.id}/education/${editEducationCommand.educationId}`;
    return this.httpClient.put<any>(url, editEducationCommand)
      .pipe(
        map(() => {
          const index = this.currentFreelancer.educations.findIndex(c => c.id === editEducationCommand.educationId);
          if (index != -1) {
            const education = Object.assign(new Education(), this.currentFreelancer.educations[index]);
            education.update(editEducationCommand);
            this.currentFreelancer.educations[index] = education;
          }
        })
      );
  }

  deleteEducation(educationId: string): Observable<any> {
    return this.httpClient.delete<any>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/education/${educationId}`)
      .pipe(
        map(() => {
          const index = this.currentFreelancer.educations.findIndex(c => c.id === educationId);
          if (index != -1)
            this.currentFreelancer.educations.splice(index, 1);
        })
      );
  }

  addCertification(addCertificationCommand: AddCertificationCommand): Observable<void> {
    return this.httpClient.post<Certification>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/certification`, addCertificationCommand)
      .pipe(
        map((certification) => {
          this.currentFreelancer.certifications.push(certification);
        })
      );
  }

  editCertification(editCertificationCommand: EditCertificationCommand): Observable<void> {
    const url = `api/freelancer-service/freelancer/${this.currentFreelancer.id}/certification/${editCertificationCommand.certificationId}`;
    return this.httpClient.put<any>(url, editCertificationCommand)
      .pipe(
        map(() => {
          const index = this.currentFreelancer.certifications.findIndex(c => c.id === editCertificationCommand.certificationId);
          if (index != -1) {
            const certification = Object.assign(new Certification(), this.currentFreelancer.certifications[index]);
            certification.update(editCertificationCommand);
            this.currentFreelancer.certifications[index] = certification;
          }
        })
      );
  }

  deleteCertification(certificationId: string): Observable<any> {
    return this.httpClient.delete<any>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/certification/${certificationId}`)
      .pipe(
        map(() => {
          const index = this.currentFreelancer.certifications.findIndex(c => c.id === certificationId);
          if (index != -1)
            this.currentFreelancer.certifications.splice(index, 1);
        })
      );
  }

  addEmployment(addEmploymentCommand: AddEmploymentCommand): Observable<void> {
    return this.httpClient.post<Employment>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/employment`, addEmploymentCommand)
      .pipe(
        map((employment) => {
          this.currentFreelancer.employments.push(employment);
        })
      );
  }

  editEmployment(editEmploymentCommand: EditEmploymentCommand): Observable<void> {
    const url = `api/freelancer-service/freelancer/${this.currentFreelancer.id}/employment/${editEmploymentCommand.employmentId}`
    return this.httpClient.put<any>(url, editEmploymentCommand)
      .pipe(
        map(() => {
          const index = this.currentFreelancer.employments.findIndex(c => c.id === editEmploymentCommand.employmentId);
          if (index != -1) {
            const employment = Object.assign(new Employment(), this.currentFreelancer.employments[index]);
            employment.update(editEmploymentCommand);
            this.currentFreelancer.employments[index] = employment;
          }
        })
      );
  }

  deleteEmployment(employmentId: string): Observable<void> {
    return this.httpClient.delete<any>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/employment/${employmentId}`)
      .pipe(
        map(() => {
          const index = this.currentFreelancer.employments.findIndex(c => c.id === employmentId);
          if (index != -1)
            this.currentFreelancer.employments.splice(index, 1);
        })
      );
  }

  addSkills(addSkillsCommand: AddSkillCommand): Observable<void> {
    return this.httpClient.put<Skill[]>(`api/freelancer-service/freelancer/${this.currentFreelancer.id}/skill`, addSkillsCommand)
      .pipe(
        map((skills) => {
          this.currentFreelancer.skills = skills;
        })
      );
  }

}
