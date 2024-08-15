export const environment = {
    Cognito: {
      userPoolId: "eu-central-1_yP2OhxI3R",
      userPoolClientId: "423ebmefbtlmsquaubaok92dtb",
      signUpVerificationMethod: "code",
      userAttributes: {
        email: {
          required: true,
        },
      },
      allowGuestAccess: true,
    },
};
  