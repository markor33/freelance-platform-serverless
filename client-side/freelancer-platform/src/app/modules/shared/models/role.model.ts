export enum Role {
  Freelancer = 0,
  Employeer = 1
}

export function getRoleFromString(str: string): Role | undefined {
  return Role[str as keyof typeof Role];
}
