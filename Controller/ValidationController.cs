using BusinessLogic;
using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static BusinessLogic.ValidationLogic;

namespace Controller
{
    public static class ValidationController
    {




        public static string AdminPanelValidationMessageTitle(string title)
        {
            return ValidationLogic.AdminPanelValidateTitle(title);
        }

        public static string AdminPanelValidationMessageDescription(string desc)
        {
            return ValidationLogic.AdminPanelValidateDescription(desc);
        }

        public static string AdminPanelValidationMessageDifficulty(string diff)
        {
            return ValidationLogic.AdminPanelValidateDifficulty(diff);
        }
     
        public static string AdminPanelValidationMessageMidiFile(MidiFile file)
        {
            return ValidationLogic.AdminPanelValidateMidiFile(file);
        }


    }
}
