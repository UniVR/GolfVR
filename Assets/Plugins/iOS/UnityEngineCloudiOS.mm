//
//  UnityEngineCloudiOS.mm
//
//  Created by Viraf Zack on 7/2/14
//  Copyright (c) 2014 Unity. All rights reserved.
//

#if __has_feature(objc_arc)
#define RETAIN self
#define AUTORELEASE self
#define RELEASE self
#define DEALLOC self
#else
#define RETAIN retain
#define AUTORELEASE autorelease
#define RELEASE release
#define DEALLOC dealloc
#endif

#import <Foundation/Foundation.h>

@interface UnityEngineCloudUtil : NSObject
+(const char*) makeStringCopy:(NSString*)nsString;
+(NSString*) createNSString:(const char*)string;
@end

@implementation UnityEngineCloudUtil

+(const char*) makeStringCopy:(NSString*)nsString
{
    const char* utf8Val = nsString ? [nsString UTF8String] : "";
	char* res = (char*)malloc(strlen(utf8Val) + 1);
	strcpy(res, utf8Val);
	return res;
}

// Converts C style string to NSString
+(NSString*) createNSString:(const char*)string
{
    if (string)
    {
        return [NSString stringWithUTF8String: string];
    }
    return nil;
}
@end



/////
@interface UnityEngineCloudMobileProvisionUtil : NSObject
+(NSDictionary*) getMobileProvision;
@end

@implementation UnityEngineCloudMobileProvisionUtil

+(NSString*) getMobileProvisionFilePath
{
    return [[NSBundle mainBundle] pathForResource:@"embedded" ofType:@"mobileprovision"];
}

+(NSString*) getMobileProvisionAllContent
{
    NSString* mobileProvisionFilePath = [UnityEngineCloudMobileProvisionUtil getMobileProvisionFilePath];
    if (mobileProvisionFilePath==nil)
        return nil;
    
    return [NSString stringWithContentsOfFile:mobileProvisionFilePath
                                     encoding:NSISOLatin1StringEncoding error:NULL];
}

+(NSString*) getMobileProvisionPlistContent
{
    NSString* mobileProvisionContent = [UnityEngineCloudMobileProvisionUtil getMobileProvisionAllContent];
    if (mobileProvisionContent==nil)
        return nil;
    
    NSScanner* plistScanner = [NSScanner scannerWithString:mobileProvisionContent];
    if ([plistScanner scanUpToString:@"<plist" intoString:nil]==NO)
        return nil;
    
    NSString* plistContent;
    if ([plistScanner scanUpToString:@"</plist>" intoString:&plistContent]==NO)
        return nil;
    
    return [NSString stringWithFormat:@"%@</plist>", plistContent];
}

+(NSDictionary*) getMobileProvision
{
    NSString* mobileProvisionPlistContent = [UnityEngineCloudMobileProvisionUtil getMobileProvisionPlistContent];
    if (mobileProvisionPlistContent==nil)
        return nil;
    
    NSData* plistData = [mobileProvisionPlistContent dataUsingEncoding:NSISOLatin1StringEncoding];
    NSError* error = nil;
    return [NSPropertyListSerialization propertyListWithData:plistData
                                                     options:NSPropertyListImmutable format:NULL error:&error];
}

@end


/////
extern "C" {
    
    static NSDictionary* mobileProvision = nil;
    static bool lookedForMobileProvision = NO;
    
    const char* UnityEngine_Cloud_GetAppInstallMode()
    {
#if TARGET_IPHONE_SIMULATOR
        NSString* value = @"simulator";
#else
        NSString* value = @"store";
        
        if (mobileProvision==nil)
        {
            if(lookedForMobileProvision)
                return NULL;
            lookedForMobileProvision = YES;
            mobileProvision = [UnityEngineCloudMobileProvisionUtil getMobileProvision];
            if (mobileProvision==nil)
                return NULL;
            [mobileProvision RETAIN];
        }

        if ([mobileProvision count])
        {
            if ([[mobileProvision objectForKey:@"ProvisionsAllDevices"] boolValue])
                value = @"enterprise";
            else if ([mobileProvision objectForKey:@"ProvisionedDevices"] &&
                     [[mobileProvision objectForKey:@"ProvisionedDevices"] count] > 0)
            {
                NSDictionary* entitlements = [mobileProvision objectForKey:@"Entitlements"];
                if ([[entitlements objectForKey:@"get-task-allow"] boolValue])
                    value = @"dev_release";
                else
                    value = @"adhoc";
            }
        }
#endif
        return [UnityEngineCloudUtil makeStringCopy:value];
        
    }
    
    const char* UnityEngine_Cloud_GetAppVersion()
    {
        // get the app version
        NSString* shortVersion = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleShortVersionString"];
        NSString* longVersion = [[NSBundle mainBundle] objectForInfoDictionaryKey:(NSString*)kCFBundleVersionKey];
        
        NSString *version = nil;
        if (longVersion) {
            version = longVersion;
        } else if (shortVersion) {
            version = shortVersion;
        } else {
            return NULL;
        }
        return [UnityEngineCloudUtil makeStringCopy:version];
    }
    
    const char* UnityEngine_Cloud_GetBundleIdentifier()
    {
        NSString* bundleIdentifier = [[NSBundle mainBundle] bundleIdentifier];
        return [UnityEngineCloudUtil makeStringCopy:bundleIdentifier];
    }
    
    bool UnityEngine_Cloud_IsJailbroken()
    {
#if TARGET_IPHONE_SIMULATOR
        return NO;
#else
        BOOL isBroken = NO;
        NSFileManager* fileManager = [NSFileManager defaultManager];
        if ([fileManager fileExistsAtPath:@"/private/var/lib/apt/"])
            return YES;
        FILE* f = fopen("/bin/bash", "r");
        if (f != NULL)
            isBroken = YES;
        fclose(f);
        return isBroken;
#endif
    }
}

