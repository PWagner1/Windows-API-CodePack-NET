// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.WindowsAPICodePack.ExtendedLinguisticServices;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ELSSamples
{

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Program
    {

        public static void Main(string[] args)
        {
            if (!MappingService.IsPlatformSupported)
            {
                Console.WriteLine(@"This demo requires to be run on at least Windows 7");
                return;
            }

            UsageSamples();
            Console.Write(@"Press any key to continue . . .");
            Console.ReadKey();
        }

        public static void UsageSamples()
        {
            /*
            Getting LAD, calling RecognizeText, exam the result with formatter, deal with exceptions, and cleanup.
            Getting SD, calling RecongizeText, exam each data ranges with formatter, and cleanup.
            Getting all services of transliteration, output their descriptions to a console.
            Getting the transliteration service which supports Cyrillic to Latin, and deal with exceptions.
            Async version for #2. The code should deal with exceptions.
            */
            LADUsageSample();
            SDUsageSample();
            TransliterationEnumSample();
            CyrlToLatinTransUsageSample1();
            CyrlToLatinTransUsageSample2();
            SDUsageSampleAsync();
        }

        public static void LADUsageSample()
        {
            try
            {
                MappingService languageDetection = new MappingService(
                    MappingAvailableServices.LanguageDetection);
                using (MappingPropertyBag bag =
                    languageDetection.RecognizeText("This is English", null))
                {
                    string[] languages = bag?.GetResultRanges()[0].FormatData(
                        new StringArrayFormatter());
                    if (languages != null)
                    {
                        foreach (string language in languages)
                        {
                            Console.WriteLine($@"Recognized language: {language}");
                        }
                    }
                }
            }
            catch (LinguisticException exc)
            {
                Console.WriteLine($@"Error calling ELS: {exc.ResultState.ErrorMessage}, HResult: {exc.ResultState.HResult}");
            }
        }

        public static void SDUsageSample()
        {
            try
            {
                MappingService scriptDetection = new MappingService(
                    MappingAvailableServices.ScriptDetection);
                using (MappingPropertyBag bag =
                    scriptDetection.RecognizeText("This is English. АБВГД.", null))
                {
                    MappingDataRange[] ranges = bag?.GetResultRanges();
                    if (ranges != null)
                    {
                        Console.WriteLine($@"Recognized {ranges.Length} script ranges");
                        NullTerminatedStringFormatter formatter = new NullTerminatedStringFormatter();
                        foreach (MappingDataRange range in ranges)
                        {
                            Console.WriteLine(
                                $@"Range from {range.StartIndex} to {range.EndIndex}, script {range.FormatData(formatter)}");
                        }
                    }
                }
            }
            catch (LinguisticException exc)
            {
                Console.WriteLine($@"Error calling ELS: {exc.ResultState.ErrorMessage}, HResult: {exc.ResultState.HResult}");
            }
        }

        public static void TransliterationEnumSample()
        {
            try
            {
                MappingEnumOptions enumOptions = new MappingEnumOptions();
                enumOptions.Category = "Transliteration";
                MappingService[] transliterationServices = MappingService.GetServices(enumOptions);
                foreach (MappingService service in transliterationServices)
                {
                    Console.WriteLine($@"Service: {service.Description}");
                }
            }
            catch (LinguisticException exc)
            {
                Console.WriteLine($@"Error calling ELS: {exc.ResultState.ErrorMessage}, HResult: {exc.ResultState.HResult}");
            }
        }

        public static void CyrlToLatinTransUsageSample1()
        {
            try
            {
                MappingService cyrlToLatin = new MappingService(
                    MappingAvailableServices.TransliterationCyrillicToLatin);
                using (MappingPropertyBag bag = cyrlToLatin.RecognizeText("АБВГД.", null))
                {
                    string transliterated = bag?.GetResultRanges()[0].FormatData(new StringFormatter());
                    Console.WriteLine($@"Transliterated text: {transliterated}");
                }
            }
            catch (LinguisticException exc)
            {
                Console.WriteLine($@"Error calling ELS: {exc.ResultState.ErrorMessage}, HResult: {exc.ResultState.HResult}");
            }
        }

        public static void CyrlToLatinTransUsageSample2()
        {
            try
            {
                MappingEnumOptions enumOptions = new MappingEnumOptions();
                enumOptions.InputScript = "Cyrl";
                enumOptions.OutputScript = "Latn";
                enumOptions.Category = "Transliteration";
                MappingService[] cyrlToLatin = MappingService.GetServices(enumOptions);
                using (MappingPropertyBag bag = cyrlToLatin[0].RecognizeText("АБВГД.", null))
                {
                    string transliterated = bag?.GetResultRanges()[0].FormatData(new StringFormatter());
                    Console.WriteLine($@"Transliterated text: {transliterated}");
                }
            }
            catch (LinguisticException exc)
            {
                Console.WriteLine($@"Error calling ELS: {exc.ResultState.ErrorMessage}, HResult: {exc.ResultState.HResult}");
            }
        }

        public static void SDSampleCallback(IAsyncResult iAsyncResult)
        {
            MappingRecognizeAsyncResult asyncResult =
                (MappingRecognizeAsyncResult)iAsyncResult;
            if (asyncResult.Succeeded)
            {
                try
                {
                    MappingDataRange[] ranges = asyncResult.PropertyBag?.GetResultRanges();
                    if (ranges != null)
                    {
                        Console.WriteLine($@"Recognized {ranges.Length} script ranges");
                        NullTerminatedStringFormatter formatter = new NullTerminatedStringFormatter();
                        foreach (MappingDataRange range in ranges)
                        {
                            Console.WriteLine(
                                $@"Range from {range.StartIndex} to {range.EndIndex}, script {range.FormatData(formatter)}, text ""{asyncResult.Text.Substring(range.StartIndex, (range.EndIndex - range.StartIndex + 1))}""");
                        }
                    }
                }
                finally
                {
                    asyncResult.PropertyBag?.Dispose();
                }
            }
            else
            {
                Console.WriteLine($@"Error calling ELS: {asyncResult.ResultState.ErrorMessage}, HResult: {asyncResult.ResultState.HResult}");
            }
        }

        public static void SDUsageSampleAsync()
        {
            try
            {
                MappingService scriptDetection = new MappingService(
                    MappingAvailableServices.ScriptDetection);
                MappingRecognizeAsyncResult asyncResult = scriptDetection.BeginRecognizeText(
                    "This is English. АБВГД.", null, SDSampleCallback, null);
                MappingService.EndRecognizeText(asyncResult);
            }
            catch (LinguisticException exc)
            {
                Console.WriteLine($@"Error calling ELS: {exc.ResultState.ErrorMessage}, HResult: {exc.ResultState.HResult}");
            }
        }

    }
}
